import { INetronSelectable } from "./ISelectable";
import { NetronPoint } from "./Point";
import { NetronCursors } from "./Cursors";
import { NetronRectangle } from "./Rectangle";
import { NetronElement } from "./Element";
import { INetronHoverable } from "./IHoverable";
import { NetronConnection } from "./Connection";
import { INetronTheme } from "./ITheme";
import { NetronUndoService } from "./UndoService";
import { INetronElementTemplate } from "./IElementTemplate";
import { NetronConnector } from "./Connector";
import { NetronContentChangedUndoUnit } from "./ContentChangedUndoUnit";
import { NetronDeleteConnectionUndoUnit } from "./DeleteConnectionUndoUnit";
import { NetronDeleteElementUndoUnit } from "./DeleteElementUndoUnit";
import { NetronSelectionUndoUnit } from "./SelectionUndoUnit";
import { NetronInsertElementUndoUnit } from "./InsertElementUndoUnit";
import { NetronInsertConnectionUndoUnit } from "./InsertConnectionUndoUnit";
import { NetronTransformUndoUnit } from "./TransformUndoUnit";
import { NetronSelection } from "./Selection";

export class NetronGraph {
    private _canvas: HTMLCanvasElement;
    private _context: CanvasRenderingContext2D;
    private _theme: INetronTheme;
    private _pointerPosition: NetronPoint = new NetronPoint(0, 0);
    private _shiftKey: boolean = false;
    private _undoService: NetronUndoService = new NetronUndoService();
    private _elements: NetronElement[] = [];
    private _activeTemplate: INetronElementTemplate = null;
    private _activeObject: INetronHoverable = null;
    private _newElement: NetronElement = null;
    private _newConnection: NetronConnection = null;
    private _selection: NetronSelection = null;
    private _track: boolean = false;
    private _keyCodeTable: any;
    private _isMozilla: boolean;
    private _isWebKit: boolean;

    private _mouseDownHandler: (e: MouseEvent) => void;
    private _mouseUpHandler: (e: MouseEvent) => void;
    private _mouseMoveHandler: (e: MouseEvent) => void;
    private _doubleClickHandler: (e: MouseEvent) => void;
    private _touchStartHandler: (e: TouchEvent) => void;
    private _touchEndHandler: (e: TouchEvent) => void;
    private _touchMoveHandler: (e: TouchEvent) => void;
    private _keyDownHandler: (e: KeyboardEvent) => void;
    private _keyPressHandler: (e: KeyboardEvent) => void;
    private _keyUpHandler: (e: KeyboardEvent) => void;
    /**
     * 点击空白处事件
     */
    public ClickBlack
    constructor(element: HTMLCanvasElement) {
        this._canvas = element;
        // alert(this._canvas.clientWidth)
        // alert(this._canvas.width)
        // this._canvas.width = this._canvas.clientWidth / this.devicePixelRatio;
        // this._canvas.height = this._canvas.clientHeight / this.devicePixelRatio;
        // alert(this._canvas.width)
        // alert(this._canvas.width)
        // alert(this._canvas.style.width)
        // this._canvas.width=6000
        // this._canvas.height=6000
        this._canvas.style.width = this._canvas.width + "px";
        this._canvas.style.height = this._canvas.height + "px";
        this._canvas.width = this._canvas.width * this.devicePixelRatio;
        this._canvas.height = this._canvas.height * this.devicePixelRatio;
        // alert(this._canvas.width)
        // alert(this._canvas.style.width)

        this._canvas.focus();
        this._context = this._canvas.getContext("2d");
        this._context.scale(this.devicePixelRatio, this.devicePixelRatio);

        this._theme = { background: "#fff", connection: "#000", selection: "#000", connector: "#31456b", connectorBorder: "#fff", connectorHoverBorder: "#000", connectorHover: "#0c0" };

        this._isWebKit = typeof navigator.userAgent.split("WebKit/")[1] !== "undefined";
        this._isMozilla = navigator.appVersion.indexOf('Gecko/') >= 0 || ((navigator.userAgent.indexOf("Gecko") >= 0) && !this._isWebKit && (typeof navigator.appVersion !== "undefined"));

        this._mouseDownHandler = (e: MouseEvent) => { this.mouseDown(e); };
        this._mouseUpHandler = (e: MouseEvent) => { this.mouseUp(e); };
        this._mouseMoveHandler = (e: MouseEvent) => { this.mouseMove(e); };
        this._doubleClickHandler = (e: MouseEvent) => { this.doubleClick(e); };
        this._touchStartHandler = (e: TouchEvent) => { this.touchStart(e); }
        this._touchEndHandler = (e: TouchEvent) => { this.touchEnd(e); }
        this._touchMoveHandler = (e: TouchEvent) => { this.touchMove(e); }
        this._keyDownHandler = (e: KeyboardEvent) => { this.keyDown(e); }
        this._keyPressHandler = (e: KeyboardEvent) => { this.keyPress(e); }
        this._keyUpHandler = (e: KeyboardEvent) => { this.keyUp(e); }

        this._canvas.addEventListener("mousedown", this._mouseDownHandler, false);
        // this._canvas.addEventListener("mouseup", this._mouseUpHandler, false);
        // this._canvas.addEventListener("mousemove", this._mouseMoveHandler, false);
        // this._canvas.addEventListener("touchstart", this._touchStartHandler, false);
        // this._canvas.addEventListener("touchend", this._touchEndHandler, false);
        // this._canvas.addEventListener("touchmove", this._touchMoveHandler, false);
        this._canvas.addEventListener("click", this._doubleClickHandler, false);
        // this._canvas.addEventListener("dblclick", this._doubleClickHandler, false);
        // this._canvas.addEventListener("keydown", this._keyDownHandler, false);
        // this._canvas.addEventListener("keypress", this._keyPressHandler, false);
        // this._canvas.addEventListener("keyup", this._keyUpHandler, false);
    }

    public dispose() {
        if (this._canvas !== null) {
            this._canvas.removeEventListener("mousedown", this._mouseDownHandler);
            this._canvas.removeEventListener("mouseup", this._mouseUpHandler);
            this._canvas.removeEventListener("mousemove", this._mouseMoveHandler);
            this._canvas.removeEventListener("click", this._doubleClickHandler);
            // this._canvas.removeEventListener("dblclick", this._doubleClickHandler);
            this._canvas.removeEventListener("touchstart", this._touchStartHandler);
            this._canvas.removeEventListener("touchend", this._touchEndHandler);
            this._canvas.removeEventListener("touchmove", this._touchMoveHandler);
            this._canvas.removeEventListener("keydown", this._keyDownHandler);
            this._canvas.removeEventListener("keypress", this._keyPressHandler);
            this._canvas.removeEventListener("keyup", this._keyUpHandler);
            this._canvas.style.background = this.theme.background;
            this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
            this._canvas = null;
            this._context = null;
        }
    }

    public get theme(): INetronTheme {
        return this._theme;
    }

    public set theme(value: INetronTheme) {
        this._theme = value;
    }

    public get elements(): NetronElement[] {
        return this._elements;
    }

    public addElement(template: INetronElementTemplate, point: NetronPoint, content: any, object: any): NetronElement {
        this._activeTemplate = template;
        let element: NetronElement = new NetronElement(template, point);
        element.content = content;
        element.insertInto(this);
        element.invalidate();
        element.Object = object;
        return element;
    }

    public createElement(template: INetronElementTemplate) {
        this._activeTemplate = template;

        this._newElement = new NetronElement(template, this._pointerPosition);
        this.update();

        this._canvas.focus();
    }

    public addConnection(connector1: NetronConnector, connector2: NetronConnector): NetronConnection {
        let connection: NetronConnection = new NetronConnection(connector1, connector2);
        connector1.connections.push(connection);
        connector2.connections.push(connection);
        connector1.invalidate();
        connector2.invalidate();
        connection.invalidate();
        return connection;
    }

    public setElementContent(element: NetronElement, content: any) {
        this._undoService.begin();
        this._undoService.add(new NetronContentChangedUndoUnit(element, content));
        this._undoService.commit();
        this.update();
    }

    public deleteSelection() {
        this._undoService.begin();

        let deletedConnections: NetronConnection[] = [];
        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            for (let j: number = 0; j < element.connectors.length; j++) {
                let connector: NetronConnector = element.connectors[j];
                for (let k: number = 0; k < connector.connections.length; k++) {
                    let connection: NetronConnection = connector.connections[k];
                    if ((element.selected || connection.selected) && (!deletedConnections.contains(connection))) {
                        this._undoService.add(new NetronDeleteConnectionUndoUnit(connection));
                        deletedConnections.push(connection);
                    }
                }
            }
        }

        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            if (element.selected) {
                this._undoService.add(new NetronDeleteElementUndoUnit(element));
            }
        }

        this._undoService.commit();
    }


    public get devicePixelRatio(): number {
        return (('devicePixelRatio' in window) && (window.devicePixelRatio > 1)) ? window.devicePixelRatio : 1;
    }

    private mouseDown(e: MouseEvent) {
        e.preventDefault();
        this._canvas.focus();
        // this.updateMousePosition(e);

        if (e.button === 0) // left-click
        {
            // alt+click allows fast creation of element using the active template
            if ((this._newElement === null) && (e.altKey)) {
                this.createElement(this._activeTemplate);
            }

            this.pointerDown();
        }
    }

    private mouseUp(e: MouseEvent) {
        e.preventDefault();
        this.updateMousePosition(e);
        if (e.button === 0) // left-click
        {
            this.pointerUp();
        }
    }

    private mouseMove(e: MouseEvent) {
        e.preventDefault();
        // this.updateMousePosition(e);
        this.pointerMove(e);
    }

    private doubleClick(e: MouseEvent) {
        e.preventDefault();
        this.updateMousePosition(e);

        if (e.button === 0) // left-click
        {
            let point: NetronPoint = this._pointerPosition;

            this.updateActiveObject(point);
            if ((this._activeObject !== null) && (this._activeObject instanceof NetronElement)) {
                let element: NetronElement = <NetronElement>this._activeObject;
                if ((element.template !== null) && ("edit" in element.template)) {
                    element.template.edit(element, this._context, point);
                    this.update();
                }
            }
        }
    }

    private touchStart(e: TouchEvent) {
        if (e.touches.length == 1) {
            e.preventDefault();
            this.updateTouchPosition(e);
            this.pointerDown();
        }
    }

    private touchEnd(e: TouchEvent) {
        e.preventDefault();
        this.pointerUp();
    }

    private touchMove(e: TouchEvent) {
        if (e.touches.length == 1) {
            e.preventDefault();
            this.updateTouchPosition(e);
            this.pointerMove(e);
        }
    }

    private pointerDown() {
        let point: NetronPoint = this._pointerPosition;
        if (this._newElement !== null) {
            this._undoService.begin();
            this._newElement.invalidate();
            this._newElement.rectangle = new NetronRectangle(point.x, point.y, this._newElement.rectangle.width, this._newElement.rectangle.height);
            this._newElement.invalidate();
            this._undoService.add(new NetronInsertElementUndoUnit(this._newElement, this));
            this._undoService.commit();
            this._newElement = null;
        }
        else {
            this._selection = null;

            this.updateActiveObject(point);
            if (this._activeObject === null) {
                // 点的空白处
                this._selection = new NetronSelection(point);
                if (this.ClickBlack != null) {
                    this.ClickBlack()
                }
            }
            else {
                // start connection
                if ((this._activeObject instanceof NetronConnector) && (!this._shiftKey)) {
                    let connector: NetronConnector = <NetronConnector>this._activeObject;
                    if (connector.isAssignable(null)) {
                        this._newConnection = new NetronConnection(connector, null);
                        this._newConnection.updateToPoint(point);
                        connector.invalidate();
                    }
                }
                else {
                    // select object
                    let selectable: INetronSelectable = <INetronSelectable>this._activeObject;
                    if (!selectable.selected) {
                        this._undoService.begin();
                        let selectionUndoUnit: NetronSelectionUndoUnit = new NetronSelectionUndoUnit();
                        if (!this._shiftKey) {
                            this.deselectAll(selectionUndoUnit);
                        }
                        selectionUndoUnit.select(selectable);
                        this._undoService.add(selectionUndoUnit);
                        this._undoService.commit();
                    }
                    else if (this._shiftKey) {
                        this._undoService.begin();
                        let deselectUndoUnit: NetronSelectionUndoUnit = new NetronSelectionUndoUnit();
                        deselectUndoUnit.deselect(selectable);
                        this._undoService.add(deselectUndoUnit);
                        this._undoService.commit();
                    }

                    // start tracking
                    let hit = new NetronPoint(0, 0);
                    if (this._activeObject instanceof NetronElement) {
                        let element: NetronElement = <NetronElement>this._activeObject;
                        hit = element.tracker.hitTest(point);
                    }
                    for (let i = 0; i < this._elements.length; i++) {
                        let element: NetronElement = this._elements[i];
                        if (element.tracker !== null) {
                            element.tracker.start(point, hit);
                        }
                    }

                    this._track = true;
                }
            }
        }

        this.update();
        this.updateMouseCursor();
    }

    private pointerUp() {
        let point: NetronPoint = this._pointerPosition;

        if (this._newConnection !== null) {
            this.updateActiveObject(point);
            this._newConnection.invalidate();
            if ((this._activeObject !== null) && (this._activeObject instanceof NetronConnector)) {
                let connector: NetronConnector = <NetronConnector>this._activeObject;
                if ((connector != this._newConnection.from) && (connector.isAssignable(this._newConnection.from))) {
                    this._undoService.begin();
                    this._undoService.add(new NetronInsertConnectionUndoUnit(this._newConnection, this._newConnection.from, connector));
                    this._undoService.commit();
                }
            }

            this._newConnection = null;
        }

        if (this._selection !== null) {
            this._undoService.begin();

            let selectionUndoUnit: NetronSelectionUndoUnit = new NetronSelectionUndoUnit();

            let rectangle: NetronRectangle = this._selection.rectangle;
            let selectable: INetronSelectable = <INetronSelectable>this._activeObject;
            if ((this._activeObject === null) || (!selectable.selected)) {
                if (!this._shiftKey) {
                    this.deselectAll(selectionUndoUnit);
                }
            }

            if ((rectangle.width !== 0) || (rectangle.height !== 0)) {
                this.selectAll(selectionUndoUnit, rectangle);
            }

            this._undoService.add(selectionUndoUnit);
            this._undoService.commit();
            this._selection = null;
        }

        if (this._track) {
            this._undoService.begin();
            for (let i = 0; i < this._elements.length; i++) {
                let element: NetronElement = this._elements[i];
                if (element.tracker !== null) {
                    element.tracker.stop();
                    element.invalidate();
                    let r1: NetronRectangle = element.rectangle;
                    let r2: NetronRectangle = element.tracker.rectangle;
                    if ((r1.x != r2.x) || (r1.y != r2.y) || (r1.width != r2.width) || (r1.height != r2.height)) {
                        this._undoService.add(new NetronTransformUndoUnit(element, r1, r2));
                    }
                }
            }

            this._undoService.commit();
            this._track = false;
            this.updateActiveObject(point);
        }

        this.update();
        this.updateMouseCursor();
    }

    private pointerMove(e) {
        let point: NetronPoint = this._pointerPosition;
        // console.log(point)
        if (this._newElement !== null) {
            // placing new element
            this._newElement.invalidate();
            // this._newElement.rectangle = new NetronRectangle(point.x, point.y, this._newElement.rectangle.width, this._newElement.rectangle.height);
            this._newElement.rectangle = new NetronRectangle(e.offsetX, e.offsetY + 50, this._newElement.rectangle.width, this._newElement.rectangle.height);
            this._newElement.invalidate();
        }

        if (this._track) {
            // moving selected elements
            for (let i: number = 0; i < this._elements.length; i++) {
                let element: NetronElement = this._elements[i];
                if (element.tracker !== null) {
                    element.invalidate();
                    element.tracker.move(point);
                    element.invalidate();
                }
            }
        }

        if (this._newConnection !== null) {
            // connecting two connectors
            this._newConnection.invalidate();
            this._newConnection.updateToPoint(point);
            this._newConnection.invalidate();
        }

        if (this._selection !== null) {
            this._selection.updateCurrentPoint(point);
        }

        this.updateActiveObject(point);
        this.update();
        this.updateMouseCursor();
    }


    private keyDown(e: KeyboardEvent) {
        if (!this._isMozilla) {
            this.processKey(e, e.keyCode);
        }
    }

    private keyPress(e: KeyboardEvent) {
        if (this._isMozilla) {
            if (typeof this._keyCodeTable === "undefined") {
                this._keyCodeTable = [];
                let charCodeTable: any = {
                    32: ' ', 48: '0', 49: '1', 50: '2', 51: '3', 52: '4', 53: '5', 54: '6', 55: '7', 56: '8', 57: '9', 59: ';', 61: '=',
                    65: 'a', 66: 'b', 67: 'c', 68: 'd', 69: 'e', 70: 'f', 71: 'g', 72: 'h', 73: 'i', 74: 'j', 75: 'k', 76: 'l', 77: 'm', 78: 'n', 79: 'o', 80: 'p', 81: 'q', 82: 'r', 83: 's', 84: 't', 85: 'u', 86: 'v', 87: 'w', 88: 'x', 89: 'y', 90: 'z',
                    107: '+', 109: '-', 110: '.', 188: ',', 190: '.', 191: '/', 192: '`', 219: '[', 220: '\\', 221: ']', 222: '\"'
                }

                for (let keyCode in charCodeTable) {
                    let key: string = charCodeTable[keyCode];
                    this._keyCodeTable[key.charCodeAt(0)] = keyCode;
                    if (key.toUpperCase() != key) {
                        this._keyCodeTable[key.toUpperCase().charCodeAt(0)] = keyCode;
                    }
                }
            }

            this.processKey(e, (this._keyCodeTable[e.charCode]) ? this._keyCodeTable[e.charCode] : e.keyCode);
        }
    }

    private keyUp(e: KeyboardEvent) {
        this.updateMouseCursor();
    }

    private processKey(e: KeyboardEvent, keyCode: number) {
        if ((e.ctrlKey || e.metaKey) && !e.altKey) // ctrl or option
        {
            if (keyCode == 65) // A - select all
            {
                this._undoService.begin();
                let selectionUndoUnit = new NetronSelectionUndoUnit();
                this.selectAll(selectionUndoUnit, null);
                this._undoService.add(selectionUndoUnit);
                this._undoService.commit();
                this.update();
                this.updateActiveObject(this._pointerPosition);
                this.updateMouseCursor();
                this.stopEvent(e);
            }

            if ((keyCode == 90) && (!e.shiftKey)) // Z - undo
            {
                this._undoService.undo();
                this.update();
                this.updateActiveObject(this._pointerPosition);
                this.updateMouseCursor();
                this.stopEvent(e);
            }

            if (((keyCode == 90) && (e.shiftKey)) || (keyCode == 89)) // Y - redo
            {
                this._undoService.redo();
                this.update();
                this.updateActiveObject(this._pointerPosition);
                this.updateMouseCursor();
                this.stopEvent(e);
            }
        }

        if ((keyCode == 46) || (keyCode == 8)) // DEL - delete
        {
            this.deleteSelection();
            this.update();
            this.updateActiveObject(this._pointerPosition);
            this.updateMouseCursor();
            this.stopEvent(e);
        }

        if (keyCode == 27) // ESC
        {
            this._newElement = null;
            this._newConnection = null;

            this._track = false;
            for (let i: number = 0; i < this._elements.length; i++) {
                let element = this._elements[i];
                if (element.tracker !== null) {
                    element.tracker.stop();
                }
            }

            this.update();
            this.updateActiveObject(this._pointerPosition);
            this.updateMouseCursor();
            this.stopEvent(e);
        }
    }

    private stopEvent(e: Event) {
        e.preventDefault();
        e.stopPropagation();
    }

    private selectAll(selectionUndoUnit: NetronSelectionUndoUnit, rectangle: NetronRectangle) {
        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            if ((rectangle === null) || (element.hitTest(rectangle))) {
                selectionUndoUnit.select(element);
            }

            for (let j: number = 0; j < element.connectors.length; j++) {
                let connector: NetronConnector = element.connectors[j];
                for (let k: number = 0; k < connector.connections.length; k++) {
                    let connection: NetronConnection = connector.connections[k];
                    if ((rectangle === null) || (connection.hitTest(rectangle))) {
                        selectionUndoUnit.select(connection);
                    }
                }
            }
        }
    }

    private deselectAll(selectionUndoUnit: NetronSelectionUndoUnit) {
        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            selectionUndoUnit.deselect(element);

            for (let j: number = 0; j < element.connectors.length; j++) {
                let connector: NetronConnector = element.connectors[j];
                for (let k: number = 0; k < connector.connections.length; k++) {
                    let connection: NetronConnection = connector.connections[k];
                    selectionUndoUnit.deselect(connection);
                }
            }
        }
    }

    private updateActiveObject(point: NetronPoint) {
        let hitObject: INetronHoverable = this.hitTest(point);
        if (hitObject != this._activeObject) {
            if (this._activeObject !== null) {
                this._activeObject.hover = false;
            }

            this._activeObject = hitObject;

            if (this._activeObject !== null) {
                this._activeObject.hover = true;
            }
        }
    }

    private hitTest(point: NetronPoint): INetronHoverable {
        let rectangle: NetronRectangle = new NetronRectangle(point.x, point.y, 0, 0);

        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            for (let j: number = 0; j < element.connectors.length; j++) {
                let connector: NetronConnector = element.connectors[j];
                if (connector.hitTest(rectangle)) {
                    return connector;
                }
            }
        }

        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            if (element.hitTest(rectangle)) {
                return element;
            }
        }

        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            for (let j: number = 0; j < element.connectors.length; j++) {
                let connector: NetronConnector = element.connectors[j];
                for (let k: number = 0; k < connector.connections.length; k++) {
                    let connection: NetronConnection = connector.connections[k];
                    if (connection.hitTest(rectangle)) {
                        return connection;
                    }
                }
            }
        }

        return null;
    }

    private updateMouseCursor() {
        if (this._newConnection !== null) {
            this._canvas.style.cursor = ((this._activeObject !== null) && (this._activeObject instanceof NetronConnector)) ? this._activeObject.getCursor(this._pointerPosition) : NetronCursors.cross;
        }
        else {
            this._canvas.style.cursor = (this._activeObject !== null) ? this._activeObject.getCursor(this._pointerPosition) : NetronCursors.arrow;
        }
    }

    private updateMousePosition(e: MouseEvent) {
        // console.log(e)
        this._shiftKey = e.shiftKey;
        // this._pointerPosition = new NetronPoint(e.pageX, e.pageY);
        this._pointerPosition = new NetronPoint(e.offsetX, e.offsetY + 50);
        let node: HTMLElement = this._canvas;
        while (node !== null) {
            this._pointerPosition.x -= node.offsetLeft;
            this._pointerPosition.y -= node.offsetTop;
            node = <HTMLElement>node.offsetParent;
        }
    }

    private updateTouchPosition(e: TouchEvent) {
        this._shiftKey = false;
        this._pointerPosition = new NetronPoint(e.touches[0].pageX, e.touches[0].pageY);
        let node: HTMLElement = this._canvas;
        while (node !== null) {
            this._pointerPosition.x -= node.offsetLeft;
            this._pointerPosition.y -= node.offsetTop;
            node = <HTMLElement>node.offsetParent;
        }
    }
    public Clear() {
        // this._canvas.style.background = this.theme.background;
        // this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
        // console.log(this._context)
        this._context.restore()
        // this._canvas.remove()
        //console.log(this._canvas.remove())

    }
    tmpInit = null
    public update(initBackground = null) {
        this._canvas.style.background = this.theme.background;
        this._context.clearRect(0, 0, this._canvas.width, this._canvas.height);
        if (initBackground == null) initBackground = this.tmpInit
        if (initBackground != null) {
            this.tmpInit = initBackground
            initBackground()
        }
        // 绘制连接线
        let connections: NetronConnection[] = [];
        for (let i = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];
            for (let j = 0; j < element.connectors.length; j++) {
                let connector: NetronConnector = element.connectors[j];
                for (let k = 0; k < connector.connections.length; k++) {
                    let connection: NetronConnection = connector.connections[k];
                    if (connections.indexOf(connection) == -1) {
                        connection.paint(this._context);
                        connections.push(connection);
                    }
                }
            }
        }

        // 绘制每一个节点和内容以及边框
        for (let i: number = 0; i < this._elements.length; i++) {
            this._context.save();
            this._elements[i].paint(this._context);
            this._context.restore();
        }

        // 绘制热点区
        for (let i: number = 0; i < this._elements.length; i++) {
            let element: NetronElement = this._elements[i];

            // 连接点
            for (let j: number = 0; j < element.connectors.length; j++) {
                let connector: NetronConnector = element.connectors[j];

                let hover: boolean = false;
                for (let k: number = 0; k < connector.connections.length; k++) {
                    if (connector.connections[k].hover) {
                        hover = true;
                    }
                }

                if ((element.hover) || (connector.hover) || hover) {
                    connector.paint(this._context, (this._newConnection !== null) ? this._newConnection.from : null);
                }
                // else if ((this._newConnection !== null) && (connector.isAssignable(this._newConnection.from))) {
                //     connector.paint(this._context, this._newConnection.from);
                // }
            }
        }

        if (this._newElement !== null) {
            this._context.save();
            this._newElement.paint(this._context);
            this._context.restore();
        }

        if (this._newConnection !== null) {
            this._newConnection.paintTrack(this._context);
        }

        if (this._selection !== null) {
            this._context.strokeStyle = this.theme.selection;
            this._selection.paint(this._context);
        }
    }
}
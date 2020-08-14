import { NetronPoint } from "./Point";
import { NetronCursors } from "./Cursors";
import { NetronRectangle } from "./Rectangle";
import { NetronElement } from "./Element";
import { INetronHoverable } from "./IHoverable";
import { INetronConnectorTemplate } from "./IConnectorTemplate";
import { NetronConnection } from "./Connection";

export class NetronConnector implements INetronHoverable {
    private _element: any;
    private _template: INetronConnectorTemplate;
    private _connections: NetronConnection[] = [];
    private _hover: boolean = false;

    constructor(element: NetronElement, template: INetronConnectorTemplate) {
        this._element = element;
        this._template = template;
    }

    private getRectangle(): NetronRectangle {
        var point: NetronPoint = this.element.getConnectorPosition(this);
        var Rectangle: NetronRectangle = new NetronRectangle(point.x, point.y, 0, 0);
        Rectangle.inflate(3, 3);
        return Rectangle;
    }

    public get element(): NetronElement {
        return this._element;
    }

    public get template(): INetronConnectorTemplate {
        return this._template;
    }

    public get connections(): NetronConnection[] {
        return this._connections;
    }

    public get hover(): boolean {
        return this._hover;
    }

    public set hover(value: boolean) {
        this._hover = value;
    }

    public getCursor(point: NetronPoint): string {
        return NetronCursors.grip;
    }

    public hitTest(rectangle: NetronRectangle): boolean {
        if ((rectangle.width === 0) && (rectangle.height === 0)) {
            return this.getRectangle().contains(rectangle.topLeft);
        }
        return rectangle.contains(this.getRectangle().topLeft);
    }

    public invalidate() {
    }

    public isAssignable(connector: NetronConnector): boolean {
        if (connector === this) {
            return false;
        }

        var t1: string[] = this._template.type.split(' ');
        if (t1.indexOf("[array]") == -1 && (this._connections.length == 1)) {
            return false;
        }

        if (connector instanceof NetronConnector) {
            var t2: string[] = connector._template.type.split(' ');
            if ((t1[0] != t2[0]) ||
                (this._element == connector._element) ||
                (t1.contains("[in]") && !t2.contains("[out]")) ||
                (t1.contains("[out]") && !t2.contains("[in]")) ||
                (!t2.contains("[array]") && (connector._connections.length == 1))) {
                return false;
            }
        }

        return true;
    }

    /**
     * 
     * @param context 绘制热点区域
     * @param other 
     */
    public paint(context: CanvasRenderingContext2D, other) {
        var rectangle: NetronRectangle = this.getRectangle();
        var strokeStyle: string = this._element.graph.theme.connectorBorder;
        var fillStyle: string = this._element.graph.theme.connector;
        if (this._hover) {
            strokeStyle = this._element.graph.theme.connectorHoverBorder;
            fillStyle = this._element.graph.theme.connectorHover;
            if (!this.isAssignable(other)) {
                fillStyle = "#f00";
            }
        }

        context.lineWidth = 1;
        context.strokeStyle = strokeStyle;
        context.lineCap = "butt";
        context.fillStyle = fillStyle;
        context.fillRect(rectangle.x - 0.5, rectangle.y - 0.5, rectangle.width, rectangle.height);
        context.strokeRect(rectangle.x - 0.5, rectangle.y - 0.5, rectangle.width, rectangle.height);

        if (this._hover) {
            // Tooltip
            var text = this._template.name;
            if ("description" in this._template) {
                text = this._template.description;
            }
            context.textBaseline = "bottom";
            context.font = "8.25pt Tahoma";
            var size: TextMetrics = context.measureText(text);
            var sizeHeight = 14;
            var sizeWidth = size.width;
            var a: NetronRectangle = new NetronRectangle(rectangle.x - Math.floor(size.width / 2), rectangle.y + sizeHeight + 6, sizeWidth, sizeHeight);
            var b: NetronRectangle = new NetronRectangle(a.x, a.y, a.width, a.height);
            a.inflate(4, 1);
            context.fillStyle = "rgb(255, 255, 231)";
            context.fillRect(a.x - 0.5, a.y - 0.5, a.width, a.height);
            context.strokeStyle = "#000";
            context.lineWidth = 1;
            context.strokeRect(a.x - 0.5, a.y - 0.5, a.width, a.height);
            context.fillStyle = "#000";
            context.fillText(text, b.x, b.y + 13);
        }
    }
}

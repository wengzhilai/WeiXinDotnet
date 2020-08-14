import { NetronRectangle } from "./Rectangle";
import { INetronUndoUnit } from "./IUndoUnit";
import { NetronElement } from "./Element";

export class NetronTransformUndoUnit implements INetronUndoUnit {
    private _element: NetronElement;
    private _undoRectangle: NetronRectangle;
    private _redoRectangle: NetronRectangle;

    constructor(element: NetronElement, undoRectangle: NetronRectangle, redoRectangle: NetronRectangle) {
        this._element = element;
        this._undoRectangle = undoRectangle.clone();
        this._redoRectangle = redoRectangle.clone();
    }

    public undo() {
        this._element.rectangle = this._undoRectangle;
    }

    public redo() {
        this._element.rectangle = this._redoRectangle;
    }

    public get isEmpty(): boolean {
        return false;
    }
}

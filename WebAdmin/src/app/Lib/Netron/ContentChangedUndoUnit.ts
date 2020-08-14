import { INetronUndoUnit } from "./IUndoUnit";
import { NetronElement } from "./Element";


export class NetronContentChangedUndoUnit implements INetronUndoUnit {
    private _element: NetronElement;
    private _undoContent: any;
    private _redoContent: any;
    constructor(element: NetronElement, content: any) {
        this._element = element;
        this._undoContent = element.content;
        this._redoContent = content;
    }

    public undo() {
        this._element.content = this._undoContent;
    }

    public redo() {
        this._element.content = this._redoContent;
    }

    public get isEmpty(): boolean {
        return false;
    }
}

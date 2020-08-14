import { INetronUndoUnit } from "./IUndoUnit";
import { INetronSelectable } from "./ISelectable";

export class NetronSelectionUndoUnit implements INetronUndoUnit {
    private _states: any[] = [];

    public undo() {
        for (var i: number = 0; i < this._states.length; i++) {
            this._states[i].value.selected = this._states[i].undo;
        }
    }

    public redo() {
        for (var i: number = 0; i < this._states.length; i++) {
            this._states[i].value.selected = this._states[i].redo;
        }
    }

    public get isEmpty(): boolean {
        for (var i: number = 0; i < this._states.length; i++) {
            if (this._states[i].undo != this._states[i].redo) {
                return false;
            }
        }
        return true;
    }

    public select(value: INetronSelectable) {
        this.update(value, value.selected, true);
    }

    public deselect(value: INetronSelectable) {
        this.update(value, value.selected, false);
    }

    private update(value: INetronSelectable, undo: boolean, redo: boolean) {
        for (var i: number = 0; i < this._states.length; i++) {
            if (this._states[i].value == value) {
                this._states[i].redo = redo;
                return;
            }
        }

        this._states.push({ value: value, undo: undo, redo: redo });
    }
}

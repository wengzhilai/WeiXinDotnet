import { NetronContainerUndoUnit } from "./ContainerUndoUnit";
import { INetronUndoUnit } from "./IUndoUnit";

export class NetronUndoService {
    private _container: NetronContainerUndoUnit = null;
    private _stack: NetronContainerUndoUnit[] = [];
    private _position: number = 0;

    public begin() {
        this._container = new NetronContainerUndoUnit();
    }

    public cancel() {
        this._container = null;
    }

    public commit() {
        if (!this._container.isEmpty) {
            this._stack.splice(this._position, this._stack.length - this._position);
            this._stack.push(this._container);
            this.redo();
        }
        this._container = null;
    }

    public add(undoUnit: INetronUndoUnit) {
        this._container.add(undoUnit);
    }

    public undo() {
        if (this._position !== 0) {
            this._position--;
            this._stack[this._position].undo();
        }
    }

    public redo() {
        if ((this._stack.length !== 0) && (this._position < this._stack.length)) {
            this._stack[this._position].redo();
            this._position++;
        }
    }
}

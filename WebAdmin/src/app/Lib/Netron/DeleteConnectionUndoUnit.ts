import { INetronUndoUnit } from "./IUndoUnit";
import { NetronConnection } from "./Connection";
import { NetronConnector } from "./Connector";

export class NetronDeleteConnectionUndoUnit implements INetronUndoUnit {
    private _connection: NetronConnection;
    private _from: NetronConnector;
    private _to: NetronConnector;

    constructor(connection: NetronConnection) {
        this._connection = connection;
        this._from = connection.from;
        this._to = connection.to;
    }

    public undo() {
        this._connection.insert(this._from, this._to);
    }

    public redo() {
        this._connection.remove();
    }

    public get isEmpty(): boolean {
        return false;
    }
}

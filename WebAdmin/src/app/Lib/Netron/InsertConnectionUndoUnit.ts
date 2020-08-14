import { INetronUndoUnit } from "./IUndoUnit";
import { NetronConnection } from "./Connection";
import { NetronConnector } from "./Connector";

    export class NetronInsertConnectionUndoUnit implements INetronUndoUnit
    {   
        private _connection: NetronConnection;
        private _from: NetronConnector;
        private _to: NetronConnector;

        constructor(connection: NetronConnection, from: NetronConnector, to: NetronConnector)
        {
            this._connection = connection;
            this._from = from;
            this._to = to;
        }

        public undo()
        {
            this._connection.remove();
        }

        public redo()
        {
            this._connection.insert(this._from, this._to);
        }

        public get isEmpty(): boolean
        {
            return false;
        }
    }

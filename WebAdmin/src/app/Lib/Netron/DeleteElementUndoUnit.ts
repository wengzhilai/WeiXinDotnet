import { INetronUndoUnit } from "./IUndoUnit";
import { NetronElement } from "./Element";
import { NetronGraph } from "./Graph";

    export class NetronDeleteElementUndoUnit implements INetronUndoUnit
    {
        private _element: NetronElement;
        private _graph: NetronGraph;

        constructor(element: NetronElement)
        {
            this._element = element;
            this._graph = element.graph;
        }

        public undo()
        {
            this._element.insertInto(this._graph);
        }

        public redo()
        {
            this._element.remove();
        }

        public get isEmpty(): boolean
        {
            return false;
        }
    }

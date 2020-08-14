import { INetronUndoUnit } from "./IUndoUnit";
import { NetronElement } from "./Element";
import { NetronGraph } from "./Graph";

    export class NetronInsertElementUndoUnit implements INetronUndoUnit
    {
        private _element: NetronElement;
        private _graph: NetronGraph;

        constructor(element: NetronElement, graph: NetronGraph)
        {
            this._element = element;
            this._graph = graph;
        }

        public undo()
        {
            this._element.remove();
        }

        public redo()
        {
            this._element.insertInto(this._graph);
        }

        public get isEmpty(): boolean
        {
            return false;
        }
    }

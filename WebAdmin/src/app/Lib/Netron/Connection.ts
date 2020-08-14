import { INetronSelectable } from "./ISelectable";
import { NetronConnector } from "./Connector";
import { NetronPoint } from "./Point";
import { NetronCursors } from "./Cursors";
import { NetronRectangle } from "./Rectangle";
import { NetronLineHelper } from "./LineHelper";

export class NetronConnection implements INetronSelectable {
    private _from: NetronConnector;
    private _to: NetronConnector;
    private _toPoint: NetronPoint = null;
    private _selected: boolean;
    private _hover: boolean;

    constructor(from: NetronConnector, to: NetronConnector) {
        this._from = from;
        this._to = to;
    }

    public get from(): NetronConnector {
        return this._from;
    }

    public get to(): NetronConnector {
        return this._to;
    }

    public get selected(): boolean {
        return this._selected;
    }

    public set selected(value: boolean) {
        this._selected = value;
        this.invalidate();
    }

    public get hover(): boolean {
        return this._hover;
    }

    public set hover(value: boolean) {
        this._hover = value;
    }

    public updateToPoint(toPoint: NetronPoint) {
        this._toPoint = toPoint;
    }

    public remove() {
        this.invalidate();
        if ((this._from !== null) && (this._from.connections.contains(this))) {
            this._from.connections.remove(this);
        }
        if ((this._to !== null) && (this._to.connections.contains(this))) {
            this._to.connections.remove(this);
        }
        this._from = null;
        this._to = null;
    }

    public insert(from: NetronConnector, to: NetronConnector) {
        this._from = from;
        this._to = to;
        this._from.connections.push(this);
        this._from.invalidate();
        this._to.connections.push(this);
        this._to.invalidate();
        this.invalidate();
    }

    public getCursor(point: NetronPoint): string {
        return NetronCursors.select;
    }

    public hitTest(rectangle: NetronRectangle): boolean {
        if ((this.from !== null) && (this.to !== null)) {
            var p1: NetronPoint = this.from.element.getConnectorPosition(this.from);
            var p2: NetronPoint = this.to.element.getConnectorPosition(this.to);
            if ((rectangle.width !== 0) || (rectangle.height !== 0)) {
                return (rectangle.contains(p1) && rectangle.contains(p2));
            }

            var p: NetronPoint = rectangle.topLeft;

            // p1 must be the leftmost NetronPoint
            if (p1.x > p2.x) { var temp = p2; p2 = p1; p1 = temp; }

            var r1: NetronRectangle = new NetronRectangle(p1.x, p1.y, 0, 0);
            var r2: NetronRectangle = new NetronRectangle(p2.x, p2.y, 0, 0);
            r1.inflate(3, 3);
            r2.inflate(3, 3);

            if (r1.union(r2).contains(p)) {
                if ((p1.x == p2.x) || (p1.y == p2.y)) // straight line
                {
                    return true;
                }
                else if (p1.y < p2.y) {
                    var o1 = r1.x + (((r2.x - r1.x) * (p.y - (r1.y + r1.height))) / ((r2.y + r2.height) - (r1.y + r1.height)));
                    var u1 = (r1.x + r1.width) + ((((r2.x + r2.width) - (r1.x + r1.width)) * (p.y - r1.y)) / (r2.y - r1.y));
                    return ((p.x > o1) && (p.x < u1));
                }
                else {
                    var o2 = r1.x + (((r2.x - r1.x) * (p.y - r1.y)) / (r2.y - r1.y));
                    var u2 = (r1.x + r1.width) + ((((r2.x + r2.width) - (r1.x + r1.width)) * (p.y - (r1.y + r1.height))) / ((r2.y + r2.height) - (r1.y + r1.height)));
                    return ((p.x > o2) && (p.x < u2));
                }
            }
        }
        return false;
    }

    public invalidate() {
        if (this._from !== null) {
            this._from.invalidate();
        }
        if (this._to !== null) {
            this._to.invalidate();
        }
    }

    public paint(context: CanvasRenderingContext2D) {
        context.strokeStyle = this.from.element.graph.theme.connection;
        context.lineWidth = (this._hover) ? 2 : 1;
        this.paintLine(context, this._selected);
    }

    public paintTrack(context: CanvasRenderingContext2D) {
        context.strokeStyle = this.from.element.graph.theme.connection;
        context.lineWidth = 1;
        this.paintLine(context, true);
    }

    public paintLine(context: CanvasRenderingContext2D, dashed: boolean) {
        if (this._from !== null) {
            var start: NetronPoint = this._from.element.getConnectorPosition(this.from);
            var end: NetronPoint = (this._to !== null) ? this._to.element.getConnectorPosition(this.to) : this._toPoint;
            if ((start.x != end.x) || (start.y != end.y)) {
                context.beginPath();
                if (dashed) {
                    NetronLineHelper.dashedLine(context, start.x, start.y, end.x, end.y);
                }
                else {
                    context.moveTo(start.x - 0.5, start.y - 0.5);

                    context.lineTo(start.x - 0.5, end.y-((end.y-start.y)/2) - 0.5);
                    context.lineTo(end.x - 0.5, end.y-((end.y-start.y)/2) - 0.5);

                    context.lineTo(end.x - 0.5, end.y - 0.5);
                    context.lineTo(end.x - 5, end.y - 5);
                    context.lineTo(end.x + 5, end.y - 5);
                    
                    context.lineTo(end.x - 0.5, end.y - 0.5);

                    context.lineTo(end.x - 0.5, end.y-((end.y-start.y)/2) - 0.5);
                    context.lineTo(start.x - 0.5, end.y-((end.y-start.y)/2) - 0.5);


                }
                context.closePath();
                context.stroke();
            }
        }
    }
}

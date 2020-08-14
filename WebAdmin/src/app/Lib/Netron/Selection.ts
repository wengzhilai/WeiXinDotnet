import { NetronPoint } from "./Point";
import { NetronRectangle } from "./Rectangle";
import { NetronLineHelper } from "./LineHelper";

export class NetronSelection {
    private _startPoint: NetronPoint;
    private _currentPoint: NetronPoint;

    constructor(startPoint: NetronPoint) {
        this._startPoint = startPoint;
        this._currentPoint = startPoint;
    }

    public get rectangle(): NetronRectangle {
        var rectangle: NetronRectangle = new NetronRectangle(
            (this._startPoint.x <= this._currentPoint.x) ? this._startPoint.x : this._currentPoint.x,
            (this._startPoint.y <= this._currentPoint.y) ? this._startPoint.y : this._currentPoint.y,
            this._currentPoint.x - this._startPoint.x,
            this._currentPoint.y - this._startPoint.y);

        if (rectangle.width < 0) {
            rectangle.width *= -1;
        }

        if (rectangle.height < 0) {
            rectangle.height *= -1;
        }

        return rectangle;
    }

    public updateCurrentPoint(currentPoint: NetronPoint) {
        this._currentPoint = currentPoint;
    }

    public paint(context: CanvasRenderingContext2D) {
        var r: NetronRectangle = this.rectangle;
        context.lineWidth = 1;
        context.beginPath();
        NetronLineHelper.dashedLine(context, r.x - 0.5, r.y - 0.5, r.x - 0.5 + r.width, r.y - 0.5);
        NetronLineHelper.dashedLine(context, r.x - 0.5 + r.width, r.y - 0.5, r.x - 0.5 + r.width, r.y - 0.5 + r.height);
        NetronLineHelper.dashedLine(context, r.x - 0.5 + r.width, r.y - 0.5 + r.height, r.x - 0.5, r.y - 0.5 + r.height);
        NetronLineHelper.dashedLine(context, r.x - 0.5, r.y - 0.5 + r.height, r.x - 0.5, r.y - 0.5);
        context.closePath();
        context.stroke();
    }
}
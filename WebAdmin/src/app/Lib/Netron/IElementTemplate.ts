import { INetronConnectorTemplate } from "./IConnectorTemplate";
import { NetronElement } from "./Element";
import { NetronPoint } from "./Point";

export interface INetronElementTemplate {
    resizable: boolean;
    defaultWidth: number;
    defaultHeight: number;
    defaultContent: any;
    connectorTemplates: INetronConnectorTemplate[];

    paint(element: NetronElement, context: CanvasRenderingContext2D);
    edit(element: NetronElement, context: CanvasRenderingContext2D, point: NetronPoint);
}

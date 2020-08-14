import { NetronPoint } from "./Point";
import { NetronElement } from "./Element";

export interface INetronConnectorTemplate {
    name: string;
    type: string;
    description: string;

    getConnectorPosition(element: NetronElement): NetronPoint;
}

    import {NetronPoint} from "./Point";

    export interface INetronHoverable
    {
        hover: boolean;
        getCursor(point: NetronPoint): string;
    }

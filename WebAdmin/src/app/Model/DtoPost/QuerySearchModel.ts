import { KeyValuePair } from "../KeyValuePair";

export class QuerySearchModel {
    constructor() {
        
    }
    sort: string;
    order: string;
    Code: string;
    page: 0;
    rows: 0;
    OrderStr: string;
    ParaList: Array<KeyValuePair>;
    WhereList: Array<querySearchWhereModel>;
    WhereListStr: string;
    ParaListStr: string
}

export class querySearchWhereModel {
    ObjFiled: string;
    OpType: string;
    Value: string;
    FieldType: string;
    FieldName: string
}
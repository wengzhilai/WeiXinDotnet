export class DtoSaveObj<T> {
    /**
     * 保存的对像
     */
    data:T
    /**
     * 保存的字段
     */
    saveFieldList:Array<string>
    /**
     * 需要忽略的字段
     */
    ignoreFieldList:Array<string>

    /**
     * 更新条件,如果为空.则以主键为更新条件
     */
    whereList:Array<string>
}
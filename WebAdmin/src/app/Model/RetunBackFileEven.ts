import { FileModel } from "./FileModel";

// declare var wx;
/**
 * 返回图片上传完的后的事件
 * 
 * @interface RetunBackFileEven
 */
 interface RetunBackFileEven {
    (
      /**
       * 传入的文件对象
       */
      inFile: FileModel,
      /**
       * 返回的文件文件路径
       */
      url: string,
      /**
       * 返回的文件对象，如果为空表示删除该对象
       */
      fileModel: FileModel): boolean;
  }
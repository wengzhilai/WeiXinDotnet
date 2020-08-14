import { NgModule } from '@angular/core';

import { FileDropDirective } from './file-drop.directive';
import { FileSelectDirective } from './file-select.directive';

@NgModule({
  declarations: [ FileDropDirective, FileSelectDirective ],
  exports: [ FileDropDirective, FileSelectDirective ]
})
export class FileUploadModule {
}

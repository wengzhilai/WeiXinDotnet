import { Component, ViewChild, TemplateRef } from '@angular/core';
import { NbDialogService } from '@nebular/theme';
import { Fun } from './Config/Fun';

@Component({
  selector: 'app-root',  
  template: `
  <router-outlet></router-outlet>
  <ng-template #loading let-data let-ref="dialogRef">
    <nb-card>
      <nb-card-body>{{ data }}</nb-card-body>
    </nb-card>
  </ng-template>

  <ng-template #dialog let-data let-ref="dialogRef">
    <nb-card>
      <nb-card-header>{{ data.header }}</nb-card-header>
      <nb-card-body>{{ data.subHeader }}</nb-card-body>
      <nb-card-footer>
        <button nbButton (click)="ref.close()">{{data.buttons[0]}}</button>
      </nb-card-footer>
    </nb-card>
  </ng-template>
`,
})
export class AppComponent {
  @ViewChild('dialog',{static:true})
  dialog: TemplateRef<any>;

  @ViewChild('loading',{static:true})
  loading: TemplateRef<any>;
  
  constructor(private dialogService: NbDialogService) {

  }
  ngOnInit() {
    Fun.init(this.dialogService,this.loading,this.dialog);
  }
}

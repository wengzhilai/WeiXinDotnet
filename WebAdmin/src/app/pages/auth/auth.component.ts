/**
 * @license
 * Copyright Akveo. All Rights Reserved.
 * Licensed under the MIT License. See License.txt in the project root for license information.
 */
import { Component, OnDestroy } from '@angular/core';

@Component({
  selector: 'nb-auth',
  styleUrls: ['./auth.component.scss'],
  template: `
  <nb-layout>
  <nb-layout-column>
    <nb-card>
      <nb-card-header>
        <nav class="navigation">
        </nav>
      </nb-card-header>
      <nb-card-body>
        <nb-auth-block>
          <router-outlet></router-outlet>
        </nb-auth-block>
      </nb-card-body>
    </nb-card>
  </nb-layout-column>
</nb-layout>
  `,
})
export class AuthPages implements OnDestroy {

  // showcase of how to use the onAuthenticationChange method
  constructor() {

  }

  ngOnDestroy(): void {
  }
}

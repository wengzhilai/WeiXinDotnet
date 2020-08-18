import { Component, OnDestroy, OnInit } from '@angular/core';
import { NbMediaBreakpointsService, NbMenuService, NbSidebarService, NbThemeService, NbSearchService } from '@nebular/theme';

import { map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { GlobalHelper } from 'src/app/Helper/GlobalHelper';
import { Variable } from '@angular/compiler/src/render3/r3_ast';
import { Variables } from 'src/app/Config/Variables';

@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit, OnDestroy {

  private destroy$: Subject<void> = new Subject<void>();
  userPictureOnly: boolean = false;
  user: any;
  Variables = Variables;
  themes = [
    {
      value: 'default',
      name: 'Light',
    },
    {
      value: 'dark',
      name: 'Dark',
    },
    {
      value: 'cosmic',
      name: 'Cosmic',
    },
    {
      value: 'corporate',
      name: 'Corporate',
    },
  ];

  currentTheme = 'default';

  userMenu = [{ title: "个人资料", link: "user/Profile" }, { title: "退出", url: "/auth/login" }];

  constructor(private sidebarService: NbSidebarService,
    private menuService: NbMenuService,
    private themeService: NbThemeService,
    private searchService: NbSearchService) {
    this.searchService.onSearchSubmit()
      .subscribe((data: any) => {
        alert(data.term);
      })
  }

  ngOnInit() {
    this.currentTheme = this.themeService.currentTheme;

    this.user = GlobalHelper.GetUserObject();
    console.log(this.user)
    this.userPictureOnly = false
    this.user.iconFiles = Variables.ImgUrl + this.user.iconFiles.replace("\\", "/");

    this.themeService.onThemeChange()
      .pipe(
        map(({ name }) => name),
        takeUntil(this.destroy$),
      )
      .subscribe(themeName => this.currentTheme = themeName);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  changeTheme(themeName: string) {
    this.themeService.changeTheme(themeName);
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');

    return false;
  }

  navigateHome() {
    this.menuService.navigateHome();
    return false;
  }
}

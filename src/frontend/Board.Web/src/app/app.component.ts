import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BoardComponent } from './components/pages/board/board.component';
import { NavigationComponent } from './components/core/navigation/navigation.component';
import { SidebarComponent } from './components/core/sidebar/sidebar.component';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: true,
    imports: [CommonModule, BoardComponent, NavigationComponent, SidebarComponent, RouterOutlet]
})
export class AppComponent {
  title = 'BoardAppClient';
  sidebarOpen = false;

  onOpenSidebar(): void {
    this.sidebarOpen = true;
  }

  onCloseSidebar(): void {
    this.sidebarOpen = false;
  }
}

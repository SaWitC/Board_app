import { Component, EventEmitter, Output, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, TranslateModule,RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  @Output() closeSidebar = new EventEmitter<void>();
  @Output() openSidebar = new EventEmitter<void>();
  @Input() isOpen = false;

  constructor(private router: Router) {
  }

  onOpenSidebar(): void {
    this.openSidebar.emit();
  }

  onMyProjectsClick(): void {
    this.router.navigate(['/boards']);
  }

  onInfoClick(): void {
    this.router.navigate(['/info']);
  }

  onCloseClick(): void {
    this.closeSidebar.emit();
  }
}

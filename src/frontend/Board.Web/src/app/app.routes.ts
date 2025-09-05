import { Routes } from '@angular/router';
import { BoardsListComponent } from './components/pages/boards-list/boards-list.component';
import { BoardComponent } from './components/pages/board/board.component';
import { InfoComponent } from './components/pages/info/info.component';
import { AppComponent } from './app.component';

export const routes: Routes =  [{
  path: '',
  children: [
    {
      path: '',
      component: BoardComponent,
    },
    {
      path: 'boards',
      component: BoardsListComponent,
    },
    {
      path: 'info',
      component: InfoComponent,
    },
    {
      path: '**',
      redirectTo: '',
    },
  ],
}];


import { Routes } from '@angular/router';
import { BoardsListComponent } from './components/boards-list/boards-list.component';
import { BoardComponent } from './components/board/board.component';
import { InfoComponent } from './components/info/info.component';
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


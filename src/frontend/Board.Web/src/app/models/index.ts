// Экспортируем все модели для удобного импорта
export { Task } from './task.interface';
export { TaskPriority } from '../enums/task-priority.enum';
export { DragDropEvent } from './drag-drop-event.interface';
export { TaskStatusConfig } from './task-status-config.interface';
export { TASK_STATUS_CONFIG } from './task-status-config.const';
export { TaskStatus } from '../enums/task-status.enum';

// Board Item models
export { BoardItemLookupDTO } from './board-item/board-item-lookup-DTO.interface';
export { BoardItemDetailsDTO } from './board-item/board-item-details-DTO.interface';
export { UpdateBoardItemDTO } from './board-item/update-board-item-DTO.interface';
export { AddBoardItemDTO } from './board-item/add-board-item-DTO.interface';

// Board Column models
export { BoardColumnLookupDTO } from './board-column/board-column-lookup-DTO.interface';
export { BoardColumnDetailsDTO } from './board-column/board-column-details-DTO.interface';
export { UpdateBoardColumnDTO } from './board-column/update-board-column-DTO.interface';
export { AddBoardColumnDTO } from './board-column/add-board-column-DTO.interface';

// Board models
export { BoardLookupDTO } from './board/board-lookup-DTO.interface';
export { BoardDetailsDTO } from './board/board-details-DTO.interface';
export { UpdateBoardDTO } from './board/update-board-DTO.interface';
export { AddBoardDTO } from './board/add-board-DTO.interface';




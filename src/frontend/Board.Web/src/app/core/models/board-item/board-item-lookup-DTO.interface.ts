import { TaskType } from "../enums/task-type.enum";

export interface BoardItemLookupDTO {
  id: string;
  title: string;
  description: string;
  boardColumnId?: string;
  taskType: TaskType;

}

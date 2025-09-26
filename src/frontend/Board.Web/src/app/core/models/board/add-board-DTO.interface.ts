import { AddBoardColumnDTO } from "../board-column/add-board-column-DTO.interface";

export interface BoardUserDTO {
  email: string;
  role: number;
}

export interface AddBoardDTO {
  title: string;
  description: string;
  boardUsers: BoardUserDTO[];
  boardColumns: AddBoardColumnDTO[];
}

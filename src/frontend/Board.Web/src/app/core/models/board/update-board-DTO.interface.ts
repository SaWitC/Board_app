import { UserAccess } from "../enums/user-access.enum";
import { BoardUserDTO } from "./add-board-DTO.interface";

export interface UpdateBoardDTO {
  id: string;
  title: string;
  description: string;
  boardUsers: BoardUserDTO[];
  boardColumns: { id?: string; title: string; description: string; }[];
}

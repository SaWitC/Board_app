export interface OrderedBoardColumnDTO {
  id: string;
  order: number;
}

export interface UpdateOrderedBoardColumnDTO {
  OrderedBoardColumns: OrderedBoardColumnDTO[];
}

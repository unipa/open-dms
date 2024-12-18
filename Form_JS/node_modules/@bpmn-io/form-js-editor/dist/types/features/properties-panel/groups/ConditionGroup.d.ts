export function ConditionGroup(field: any, editField: any): {
    id: string;
    label: string;
    entries: {
        id: string;
        component: (props: any) => any;
        editField: any;
        field: any;
        isEdited: any;
    }[];
};

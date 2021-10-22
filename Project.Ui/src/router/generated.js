
const routers = [
    { path: 'pessoa', name: 'Cadastro > Pessoas', component: () => import('@/views/pessoa/pessoa-index') },
    { path: 'pessoa/create', name: 'Create Cadastro > Pessoas', component: () => import('@/views/pessoa/pessoa-create') },
    { path: 'pessoa/edit/:id', name: 'Edit Cadastro > Pessoas', component: () => import('@/views/pessoa/pessoa-edit') },

];

export default routers
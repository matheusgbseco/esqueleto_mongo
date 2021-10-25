<template>

    <div>

        <div class="container-fluid">

            <div class="row">
                <div class="col">
                    <div class="card shadow">
                        <div class="card-header border-0">
                            <div class="row">
                                <div class="col-xl-6">
                                    <h2>
                                        <i class="fas fa-cogs"></i>
                                        Cadastro > Produtos
                                    </h2>
                                </div>
                                <div class="col-xl-6 text-right">
                                    <div class="btn-group">
                                        <button class="btn btn-outline-default" @click="$router.back()"><i class="fas fa-arrow-left"></i> Voltar</button>
                                        <button class="btn btn-primary" @click="openFilter()"><i class="fas fa-filter"></i> Filtros</button>
                                        <!--<button class="btn btn-default" @click="openCreate()"><i class="far fa-file-excel"></i> Export</button>-->
                                        <button class="btn btn-success" @click="openCreate()"><i class="fas fa-plus"></i> Cadastrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body" v-show="dialogFilter">
                            <form v-on:keyup.enter="executeFilter(filter)">
                                <div class="row">
                                    <div class="form-group col-md-6">
                                        <label for="nome">Nome</label>
                                        <input type="text" class="form-control form-control-alternative" name="nome" placeholder="Nome" v-model="filter.nome" />
                                    </div>
                                </div>
                                <div class="text-right">
                                    <button type="button" class="btn btn-secondary" @click="executeFilter(filter)"><i class="fas fa-search"></i> Buscar</button>
                                </div>
                            </form>
                        </div>
                        <div class="table-responsive">
                            <table class="table align-items-center table-flush table-hover">
                                <thead class="thead-light">
                                    <tr>
                                        <th class="text-center" width="150"><i class="fa fa-cog"></i></th>
                                        <th>#</th>
                                        <th>Nome <i @click="executeOrderBy('Nome')" class="fa fa-sort"></i></th>
                                        <th>Categoria</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(item,index) in result.items" v-bind:key="item.produtoId" class="animated fadeIn">
                                        <td class="text-center">
                                            <button type="button" class="btn btn-sm btn-primary" @click="openEdit({ produtoId: item.produtoId })">
                                                <i class="far fa-edit"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-danger" @click="openRemove({ produtoId: item.produtoId })">
                                                <i class="far fa-trash-alt"></i>
                                            </button>
                                        </td>
                                        <td>{{ index+1 }}</td>
                                        <td>{{ item.nome }}</td>
                                        <td>{{ item.categoria }}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="card-footer py-4">
                            <div class="card-block no-padding">
                                <pagination :total="result.total" :page-size="filter.pageSize" :callback="executePageChanged"></pagination>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <b-modal :no-close-on-backdrop="true" :no-close-on-esc="true" centered v-model="dialogCreate" size="lg" :hide-footer="true" title="Cadastrar">
            <form-create v-if="dialogCreate" @on-saved="onSaved()" @on-back="hideAll()" />
        </b-modal>

        <b-modal :no-close-on-backdrop="true" :no-close-on-esc="true" centered v-model="dialogEdit" size="lg" :hide-footer="true" title="Editar">
            <form-edit v-if="dialogEdit" :id="produtoId" @on-saved="onSaved()" @on-back="hideAll()" />
        </b-modal>

        <b-modal :no-close-on-backdrop="true" :no-close-on-esc="true" centered v-model="dialogRemove" title="Confirmação">
            <p>Tem certeza que deseja remover este registro?</p>
            <template slot="modal-footer">
                <button class="btn btn-danger btn-block" @click="executeRemove()">
                    <i class="far fa-trash-alt"></i>
                    Remover
                </button>
            </template>
        </b-modal>

    </div>

</template>

<script>

    import base from '@/common/mixins/base.js'
    import Api from '@/common/api'

    import formCreate from './produto-form-create'
    import formEdit from './produto-form-edit'

    export default {
        name: 'produto',
        mixins: [base],
        components: { formCreate, formEdit },
        data() {
            return {

                dialogRemove: false,
                dialogCreate: false,
                dialogEdit: false,
                dialogFilter: false,

                model: {},
                produtoId: null,

                filter: {
                    pageSize: 50,
                    pageIndex: 1,
                    isPagination: true,
                },

                result: {
                    total: 0,
                    items: []
                }
            }
        },

        methods: {
            openFilter() {
                this.dialogFilter = !this.dialogFilter;
            },
            openEdit(model) {
                this.produtoId = model.produtoId;
                this.dialogEdit = true;
            },
            openCreate() {
                this.dialogCreate = true;
            },
            onSaved() {
                this.hideAll();
                this.executeLoad();
            },
            hideAll() {
                this.dialogCreate = false;
                this.dialogEdit = false;
                this.dialogRemove = false;
            },

            openRemove(model) {
                this.dialogRemove = true;
                this.model = model;
            },
            executeRemove(model) {
                if (model) this.model = model;
                this.showLoading();
                new Api('produto').delete(this.model).then(() => {
                    this.defaultSuccessResult();
                    this.hideAll();
                    this.hideLoading();
                    this.executeLoad();
                }, err => {
                    this.hideLoading();
                    this.defaultErrorResult(err);
                })
            },

            executeFilter(filter) {
                if (filter) this.filter = filter;
                this.hideAll();
                this.executeLoad();
            },
            executePageChanged(index) {
                this.filter = this.defaultPageChanged(this.filter, index);
                this.executeLoad();
            },
            executeOrderBy(field) {
                this.filter = this.defaultOrderBy(this.filter, field);
                this.executeLoad();
            },
            executeLoad() {
                this.showLoading();
                new Api('produto/GetData').get(this.filter).then(_result => {
                    if (_result.summary) this.result.total = _result.summary.total;
                    this.result.items = _result.data;
                    this.hideLoading();
                }, (err) => {
                    this.hideLoading();
                    this.defaultErrorResult(err);
                });
            },

        },
        mounted() {
            this.executeFilter();
        }


    };
</script>
import Vue from "vue";
import VueRouter from "vue-router";

import generated from "./generated"

import Auth from '@/common/auth'

Vue.use(VueRouter)

const routes = [

    {
        path: "/",
        redirect: '/home',
        component: () => import('../layout/index'),
        meta: { requiresAuth: true },
        children: [
            {
                path: '/home',
                name: '/home',
                component: () => import('../views/home')
            },
            {
                path: '/categoria',
                name: '/categoria',
                component: () => import('../views/categoria')
            },
            {
                path: '/produto',
                name: '/produto',
                component: () => import('../views/produto')
            },
        ].concat(generated),
    },

    {
        path: '/auth/signin',
        name: 'auth/signin',
        component: () => import('../views/auth/signin'),
    },
    {
        path: '/auth/signup',
        name: 'auth/signup',
        component: () => import('../views/auth/signup'),
    },
    {
        path: '/auth/verify',
        name: 'auth/verify',
        component: () => import('../views/auth/verify'),
    },
    {
        path: '/acessonegado',
        name: 'acessonegado',
        component: () => import('../views/acessonegado'),
    },
];

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes,
    scrollBehavior: to => {
        if (to.hash) {
            return { selector: to.hash };
        } else {
            return { x: 0, y: 0 };
        }
    }
})

router.beforeEach(async (to, from, next) => {

    if (to.matched.some(record => record.meta.requiresAuth)) {

        if (!Auth.logged()) {
            Auth.login(true);
            return;
        }

        if (!Auth.canAccess(to.path)) {
            next('/acessonegado')
            return;
        }

    }
    next();
});

export default router

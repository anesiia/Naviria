describe('Header navigation', () => {
    beforeEach(() => {
        cy.loginAsMaria();
        cy.visit('/'); // стартова сторінка, яка містить <Header />
    });

    it('Перевіряє переходи по кнопках у навігаційному меню', () => {
        const navLinks = [
            { text: 'Навірії', url: '/tasks' },
            { text: 'Помічник', url: '/assistant' },
            { text: 'Досягнення', url: '/achievements' },
            { text: 'Статистика', url: '/statistics' },
            { text: 'Ком\'юніті', url: '/friends' },
        ];

        navLinks.forEach(({ text, url }) => {
            cy.contains('nav a', text)
                .should('have.attr', 'href', url)
                .click();
            cy.url().should('include', url);
            cy.go('back'); // повернення назад після перевірки
        });
    });

    it('Переходить на сторінку профілю при кліку на аватар', () => {
        // Аватар — це <img> всередині <Link to="/profile">
        cy.get('a[href="/profile"] img.avatar').click();

        // Перевіряємо URL після переходу
        cy.url().should('include', '/profile');

        cy.contains('Особистий прогрес').should('exist');
    });
});

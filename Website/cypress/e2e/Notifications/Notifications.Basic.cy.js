describe('Notifications popup', () => {
    it('Показує повідомлення у правильному порядку та підтримує прокрутку', () => {
        cy.loginAsMaria();
        // Відкриваємо попап
        cy.get('button.notifications').click();

        // 1. Перевірка наявності контейнера
        cy.get('.notifications-popup').should('exist');

        // 2. Перевірка наявності елементів
        cy.get('.notification-item').should('have.length.greaterThan', 1);

        // 3. Перевірка наявності скролу (scrollHeight > clientHeight)
        cy.get('.notifications-popup').should(($el) => {
            const el = $el[0];
            expect(el.scrollHeight).to.be.greaterThan(el.clientHeight);
        });

        // 4. Перевірка порядку сповіщень (за спаданням дати)
        cy.get('.notification-item .notification-time').then(($times) => {
            const actual = [...$times].map((el) => new Date(el.innerText).getTime());
            const sorted = [...actual].sort((a, b) => b - a);
            expect(actual).to.deep.equal(sorted);
        });

        // 5. Натискаємо "Прочитати всі"
        cy.contains('button', 'Прочитати всі').click();

        // 6. Перевіряємо, що всі повідомлення прочитані
        cy.get('.notification-item.unread').should('not.exist');
    });

    it('Показує повідомлення зі словом "Підтримка" від bennbarnes та наявність його в списку друзів', () => {
        cy.loginAsNataliya();

        // Клік по іконці повідомлень
        cy.get('button.notifications').click();

        cy.contains('.notification-item', 'Підтримка')
            .should('be.visible')
            .and('contain.text', 'bennbarnes');

        cy.get('body').should('contain.text', 'bennbarnes');
    });

        it('Показує повідомлення про відсутність нових', () => {
            cy.loginAsNatastya();

            // Клік по іконці
            cy.get('button.notifications').click();

            // Перевіряємо текст про відсутність повідомлень
            cy.contains('Немає нових повідомлень').should('be.visible');
        });


});

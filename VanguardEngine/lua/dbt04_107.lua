-- フォーチュン・リーディング

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.LookAtTopOfDeck(1)
	local option = obj.SelectOption(obj.GetDescription(2), obj.GetDescription(3))
	if option == 1 then
		obj.RearrangeOnTop({q.Location, l.Looking})
	else
		obj.RearrangeOnBottom({q.Location, l.Looking})
	end
	obj.Draw(1)
end
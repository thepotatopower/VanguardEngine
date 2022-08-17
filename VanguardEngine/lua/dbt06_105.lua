-- 甘美なるは悪しき夢

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetGetCosts("GetCosts")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function GetCosts()
	return p.CB, 1
end

function Activation()
	obj.Draw(1)
	obj.LookAtTopOfDeck(1)
	obj.DisplayCards({q.Location, l.Looking})
	local option = obj.SelectOption(obj.GetDescription(2), obj.GetDescription(3))
	if option == 2 then
		obj.AddToDrop({q.Location, l.Looking})
	end
end
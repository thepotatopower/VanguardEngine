-- 世界周遊スペシャルライブツアー！

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.Order)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.OncePerTurn)
end

function Cost(check)
	if check then return obj.CanCB(2) end
	obj.CounterBlast(2)
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	local id = obj.GetID({q.Location, l.Selected})
	if id >= 0 then
		local newAbility = GiveAbility(GetID(), id)
		newAbility.SetDescription(2)
		newAbility.SetTiming(a.Cont)
		newAbility.SetLocation(l.VC)
		newAbility.SetResetTiming(p.UntilEndOfTurn)
		newAbility.SetResetTarget(id)
		newAbility.SetActivation("Cont")
	end
end

function Cont()
	obj.AddCardValue({q.Location, l.PlayerRC}, cs.BonusPower, 5000, p.Continuous)
end
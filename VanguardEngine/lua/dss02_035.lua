-- 混濁の瘴気

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
	obj.SuperiorCall({q.Location, l.Drop, q.Grade, 3, q.Other, o.GradeOrLess, q.Count, 1})
	if obj.IsAlchemagic() then
		obj.Select({q.Location, l.PlayerVC, q.Count, 1})
		local id = obj.GetID({q.Location, l.Selected})
		if id >= 0 then
			local given = GiveAbility(GetID(), id)
			given.SetDescription(2)
			given.SetTiming(a.Cont)
			given.SetLocation(l.VC)
			given.SetActivation("Cont")
			given.SetResetTarget(id)
			given.SetResetTiming(p.UntilEndOfTurn)
		end
	end
end

function Cont()
	obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end
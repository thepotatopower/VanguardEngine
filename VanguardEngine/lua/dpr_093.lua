-- 焔の巫女 エルバ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanRest({q.Location, l.PlayerRC, q.NameContains, obj.LoadName("Blaze Maiden"), q.Other, o.Standing, q.Other, o.DifferentNames, q.Count, 3}) end
	obj.CounterBlast(1)
	obj.Select("Filter", {l.PlayerRC}, 3, 3, {p.DifferentNames}, -1)
	obj.Rest({q.Location, l.Selected})
end

function Filter(id)
	return obj.NameContains(id, obj.LoadName("Blaze Maiden")) and obj.IsStanding(id)
end

function Activation()
	obj.Draw(1)
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	local id = obj.GetID({q.Location, l.Selected})
	if id >= 0 then
		local given = GiveAbility(GetID(), id)
		given.SetDescription(2)
		given.SetTiming(a.Cont)
		given.SetLocation(l.VC)
		given.SetActivation("Given")
		given.SetResetTarget(id)
		given.SetResetTiming(p.UntilEndOfTurn)
	end
end

function Given()
	obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 10000, p.Continuous)
end

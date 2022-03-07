-- 晴天の恵光 ラーシエル

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Clarissa")})
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanRetire({q.Other, o.This}) end
	obj.SoulBlast(1)
	obj.Retire({q.Other, o.This})
end

function Activation()
	obj.LookAtTopOfDeck(7)
	obj.SuperiorCallToSpecificCircle({q.Location, l.Looking, q.NameContains, obj.LoadName("Earnescorrect"), q.Count, 1, q.Min, 0}, FL.OpenCircle)
	obj.Shuffle()
end
-- 風の中の音 デュケイラ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC, a.PlacedOnVC)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.GetNumberOf("OnPlaceFilter", {l.Units}) > 0
end

function OnPlaceFilter(id)
	return obj.IsPlayer(id) and obj.IsApplicable(id) and obj.HasProperty(id, p.Friend) and obj.IsSameColumn(id)
end

function Condition()
	return obj.GetNumberOf("FriendsFilter", {l.Units}) > 0
end

function FriendsFilter(id)
	return obj.IsPlayer(id) and not obj.IsThis(id) and obj.HasCardState(id, cs.Friend)
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.Draw(1)
end
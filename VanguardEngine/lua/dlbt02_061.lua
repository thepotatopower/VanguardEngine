-- 真剣な眼差し イヴェッタ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.Friend)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- cont2
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetActivation("Cont2")
end

function Cont()
	if obj.GetNumberOf("ContFilter", {l.Units}) > 0 then
		obj.AddCardState("FriendFilter", {l.Units}, cs.Friend, p.Continuous)
	end
end

function ContFilter(id)
	return not obj.IsThis(id) and obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function FriendFilter(id)
	return obj.IsPlayer(id) and obj.IsSameColumn(id)
end

function Cont2()
	if obj.GetNumberOf("FriendExists", {l.Units}) > 0 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end

function FriendExists(id)
	return obj.IsPlayer(id) and not obj.IsThis(id) and obj.HasCardState(id, cs.Friend)
end
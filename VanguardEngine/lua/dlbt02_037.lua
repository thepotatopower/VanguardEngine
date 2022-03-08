-- チェシャースマイル ラリサ

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.Friend)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC, l.VC)
	ability1.SetActivation("Cont")
	-- on end of battle
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("Trigger")
	ability2.SetCondition("Condition")
	ability2.SetActivation("Activation")
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

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher}) and obj.IsSameZone() and obj.GetNumberOf("OnAttackFilter", {l.Units}) > 0
end

function OnAttackFilter(id)
	return obj.IsPlayer(id) and not obj.IsThis(id) and obj.HasCardState(id, cs.Friend)
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 2000 * obj.GetNumberOf("OnAttackFilter", {l.Units}), p.UntilEndOfBattle)
end
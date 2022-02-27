-- 轟然の天斬 ゲトゥーゼ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Grade, 2, q.Other, o.Attacked, q.Other, o.GradeOrHigher})
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Grade, 2, q.Other, o.Attacked, q.Other, o.GradeOrHigher, q.Count, 1})
	local power = 10000
	if obj.GetGrade({q.Location, l.Selected}) == 4 then
		power = 20000
	end
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, power, p.UntilEndOfBattle)
end
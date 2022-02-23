-- 焔の巫女 トーニャ

function RegisterAbilities()
	-- on end of battle
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerUnits, q.Name, obj.GetNameFromCardID("dbt04_063"), q.Other, o.Attacking})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	local grade = obj.GetGrade({q.Location, l.PlayerVC})
	return grade >= 0 and obj.CanSearch({q.Location, l.Deck, q.Grade, grade + 1, q.Other, o.GradeOrHigher, q.Count, 1})
end

function Activation()
	obj.Retire({q.Location, l.PlayerUnits, q.Other, o.SameColumn})
	local grade = obj.GetGrade({q.Location, l.PlayerVC})
	if grade >= 0 then
		obj.Search({q.Location, l.Deck, q.Grade, grade + 1, q.Other, o.GradeOrHigher, q.Count, 1, q.Min, 0})
	end
end
-- Penetrate Dragon, Tribash

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Count, 2, q.Other, o.OrLess
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.VanguardIsAttackingUnit() and obj.Exists(1) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.AddToSoul(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddCritical(3, 1)
	end
	return 0
end
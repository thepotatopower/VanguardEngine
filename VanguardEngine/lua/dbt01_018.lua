-- Detonation Mutant, Bobalmine

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerOrder, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBattleEnds, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsBooster() and obj.Exists(1) then
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
		obj.CounterCharge(1)
	end
	return 0
end
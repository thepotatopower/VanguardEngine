-- Dance Score, Ermel

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerOrder
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnGC, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.GetNumberOf(2) > 0 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddUntilEndOfBattleValue(1, cs.BonusShield, 5000 * obj.GetNumberOf(2))
	end
	return 0
end
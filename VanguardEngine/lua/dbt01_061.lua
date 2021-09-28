-- Stealth Fiend, Shigamanago

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Looking, q.Count, 1, q.Min, 0
	elseif n == 3 then
		return q.Location, l.Selected, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnEnemyRetired, p.HasPrompt, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.PlayerMainPhase() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.LookAtTopOfDeck(1)
		obj.Select(2)
		if obj.Exists(3) then
			obj.SuperiorCall(3)
		else
			obj.SoulCharge(1)
		end
		obj.EndSelect()
	end
	return 0
end
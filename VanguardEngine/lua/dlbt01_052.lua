-- Fulfill Sweets, Anselma

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.NotThis
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.OncePerTurn, p.SB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsBackRowRearguard() and obj.IsPlayerTurn() and obj.IsBattlePhase() and obj.Exists(1) then
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
		obj.Draw(1)
	end
	return 0
end
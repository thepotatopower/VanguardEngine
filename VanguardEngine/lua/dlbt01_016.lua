-- Earnescorrect Supporter, Riona

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRCRetired, q.NameContains, "Earnescorrect", q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnPlayerRCRetired, p.HasPrompt, p.SB, 1, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and not obj.IsPlayerTurn() and obj.Exists(1) then
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
		obj.AddToHand(1)
	end
	return 0
end
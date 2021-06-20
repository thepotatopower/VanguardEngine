-- Apex Ruler, Bastion

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3
	elseif n == 2 then
		return q.Location, l.RevealedTriggers, q.Grade, 3, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 5 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, true, true
	elseif n == 2 then
		return a.OnBattleEnds, false, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.NotActivatedYet() and obj.Exists(2) and obj.Exists(4) and obj.CanDiscard(3) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.Discard(3)
	end
end

function Activate(n)
	if n == 1 then
		if obj.IsPlayerTurn() then
			obj.SetAbilityPower(1, 2000)
		else
			obj.SetAbilityPower(1, 0)
		end
	elseif n == 2 then
		obj.Select(4)
		obj.AutoStand(5)
		obj.AddTempPower(5, 10000)
		obj.EndSelect()
	end
	return 0
end